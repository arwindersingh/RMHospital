param(
    [String]$apiHostname = "localhost",
    [int]$apiPort = 5000,
    [String]$apiPathPrefix,
    [System.Management.Automation.PSCredential]$credential
)

$messages = New-Object "System.Collections.ArrayList"
$apiPath = $apiPathPrefix + "/designation"
$uri = New-Object "System.UriBuilder" ("http", $apiHostname, $apiPort, $apiPath)
$uri = $uri.Uri

$nurseTypes = @("NUM", "CNM", "CNE", "CNC", "ANUM", "CNS", "CCRN", "Data Nurse - B0014",
    "Donation Specialist Nursing Coordinator", "MERN", "Team Leader - Group 1",
    "Team Leader - Support", "GradCert CTS", "RN Div 1", "Research Nurse (Y7891)", 
    "Transition Program 2016", "Bridging ED", "GCCP 18 Month 2017",
    "GCCP 2017", "Intro2 FEB 2017", "Bridging Sep 16", "Intro to ICU - Sept 2016",
    "Grad Cert Feb 2016 (2 years)")

foreach ($entry in $nurseTypes) {
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
