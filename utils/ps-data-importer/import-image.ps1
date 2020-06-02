param(
    [String]$filePath,
    [String]$apiHostname = "localhost",
    [int]$apiPort = 5000,
    [String]$apiPathPrefix,
    [System.Management.Automation.PSCredential]$credential
)
Add-Type -AssemblyName System.Net.Http
Add-Type -AssemblyName System.Web

$apiPath = $apiPathPrefix + "/image"
$uri = New-Object "System.UriBuilder" ("http", $apiHostname, $apiPort, $apiPath)
$uri = $uri.Uri

if ($filePath -eq "") {
    return $null
}

# Determine the content type
$contentType = [System.Web.MimeMapping]::GetMimeMapping($filePath)
if ($contentType -eq $null) {
    $contentType = "application/octet-stream"
}

# Build the HTTP Client
$httpClientHandler = New-Object System.Net.Http.HttpClientHandler
if ($credential) {
    $networkCredential = New-Object System.Net.NetworkCredential ($credential.UserName, $credential.Password)
    $httpClientHandler.Credentials = $networkCredential
}
$httpClient = New-Object System.Net.Http.Httpclient $httpClientHandler

# Build the message
$packageFileStream = New-Object System.IO.FileStream @($filePath, [System.IO.FileMode]::Open)

$contentDispositionHeaderValue = New-Object System.Net.Http.Headers.ContentDispositionHeaderValue "form-data"
$contentDispositionHeaderValue.Name = "file"
$contentDispositionHeaderValue.FileName = (Split-Path $filePath -Leaf)

$streamContent = New-Object System.Net.Http.StreamContent $packageFileStream
$streamContent.Headers.ContentDisposition = $contentDispositionHeaderValue
$streamContent.Headers.ContentType = New-Object System.Net.Http.Headers.MediaTypeHeaderValue $contentType

$content = New-Object System.Net.Http.MultipartFormDataContent
$content.Add($streamContent)

# Send the image to the API and return the ID
$response = $httpClient.PostAsync($uri, $content).Result
$responseContent = $response.Content.ReadAsStringAsync().Result
$httpClient.Dispose()
$response.Dispose()

return $responseContent
