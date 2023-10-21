# Import the CSV file
$data = Import-Csv -Path 'items.csv'

# Convert the data types
$data = $data | ForEach-Object {
    $_.itemId = [int]$_.itemId
    $_.price = [int]$_.price
	if ($_.isCraftable -ieq "true") {
		$_.isCraftable = $true
	}
	elseif ($_.isCraftable -ieq "false") {
		$_.isCraftable = $false;
	}
	else {
		$_.isCraftable = $null
	}
	
	$_
}

# Convert the data to JSON
$json = $data | ConvertTo-Json

# Output the JSON to a file
$json | Out-File -FilePath 'items.json'