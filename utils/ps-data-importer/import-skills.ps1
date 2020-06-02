param(
    [String]$apiHostname = "localhost",
    [int]$apiPort = 5000,
    [String]$apiPathPrefix,
    [System.Management.Automation.PSCredential]$credential
)

$messages = New-Object "System.Collections.ArrayList"
$apiPath = $apiPathPrefix + "/skill"
$uri = New-Object "System.UriBuilder" ("http", $apiHostname, $apiPort, $apiPath)
$uri = $uri.Uri

$skills = @("ALS", "Transport", "Mechanical Ventilation", "Advanced Ventilation",
    "Neuro", "Pulmonary Catheter", "Day 1", "Day 0", "Pacing", "CCRT", "IABP")

foreach ($entry in $skills) {
    $message = New-Object System.Object
    $message | Add-Member -Type NoteProperty -Name "name" -Value $entry
    $messages.Add($message)
}

foreach ($msg in $messages) {
    $json = $msg | ConvertTo-Json
    if ($credential) {
        Invoke-RestMethod -Uri $uri -Method Post -ContentType "application/json" -Body $json -Credential $credential
    } else {
        Invoke-RestMethod -Uri $uri -Method Post -ContentType "application/json" -Body $json
    }
}
