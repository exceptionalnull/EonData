# Import the CSV file
$data = Import-Csv -Path 'items.csv'

# Convert the data to JSON
$json = $data | ConvertTo-Json

# Output the JSON to a file
$json | Out-File -FilePath 'items.json'
