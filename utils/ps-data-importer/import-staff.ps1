param(
    [String]$csvPath = "data.csv",
    [String]$imagePath = ".\images",
    [String]$apiHostname = "localhost",
    [int]$apiPort = 5000,
    [String]$apiPathPrefix,
    [System.Management.Automation.PSCredential]$credential
)

$data = Import-Csv $csvPath
$messages = New-Object "System.Collections.ArrayList"
$apiPath = $apiPathPrefix + "/staff"
$uri = New-Object "System.UriBuilder" ("http", $apiHostname, $apiPort, $apiPath)
$uri = $uri.Uri

$gradNurseTypes = @("NUM", "CNM", "CNE", "CNC", "ANUM", "CNS", "CCRN", "Data Nurse - B0014",
    "Donation Specialist Nursing Coordinator", "MERN", "Team Leader - Group 1",
    "Team Leader - Support", "GradCert CTS", "RN Div 1", "Research Nurse (Y7891)")

foreach ($entry in $data) {
    $message = New-Object System.Object
    $name = -join ($entry.'First Name', " ", $entry.'Last Name')
    $message | Add-Member -Type NoteProperty -Name "name" -Value $name
    $message | Add-Member -Type NoteProperty -Name "staff_type" -Value $entry.Type
    $message | Add-Member -Type NoteProperty -Name "rosteron_id" -Value ([int]($entry.RosterOnID))

    $image = Join-Path -Path $imagePath -ChildPath ($entry.RosterOnID + ".jpg")
    if (Test-Path $image) {
        $imageId = (.\import-image.ps1 -filePath $image -apiHostname $apiHostname -apiPort $apiPort -apiPathPrefix $apiPathPrefix -credential $credential)
        $message | Add-Member -Type NoteProperty -Name "photo" -Value $imageId
    }

    # Add the skills
    $message | Add-Member -Type NoteProperty -Name "skills" -Value (New-Object "System.Collections.ArrayList")
    if ($entry.ALS -eq "TRUE") {
        $message.skills.Add("ALS")
    }
    if ($entry.Transport -eq "TRUE") {
        $message.skills.Add("Transport")
    }
    if ($gradNurseTypes.Contains($entry.Type) -or ($entry.'Mechanical Ventilation' -eq "TRUE")) {
        $message.skills.Add("Mechanical Ventilation")
    }
    if ($gradNurseTypes.Contains($entry.Type) -or ($entry.'Advanced Ventilation' -eq "TRUE")) {
        $message.skills.Add("Advanced Ventilation")
    }
    if ($gradNurseTypes.Contains($entry.Type) -or ($entry.Neuro -eq "TRUE")) {
        $message.skills.Add("Neuro")
    }
    if ($gradNurseTypes.Contains($entry.Type) -or ($entry.'Pulmonary Catheter' -eq "TRUE")) {
        $message.skills.Add("Pulmonary Catheter")
    }
    if ($gradNurseTypes.Contains($entry.Type) -or ($entry.'Day 1' -eq "TRUE")) {
        $message.skills.Add("Day 1")
    }
    if ($gradNurseTypes.Contains($entry.Type) -or ($entry.'Day 0' -eq "TRUE")) {
        $message.skills.Add("Day 0")
    }
    if ($gradNurseTypes.Contains($entry.Type) -or ($entry.Pacing -eq "TRUE")) {
        $message.skills.Add("Pacing")
    }
    if ($gradNurseTypes.Contains($entry.Type) -or ($entry.CCRT -eq "TRUE")) {
        $message.skills.Add("CCRT")
    }
    if ($gradNurseTypes.Contains($entry.Type) -or ($entry.IABP -eq "TRUE")) {
        $message.skills.Add("IABP")
    }
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
