$policyConfig = New-Object 'System.Collections.Generic.Dictionary[string,string]'

#deployment map
$policyConfig.Add("B2C_1A_TRUSTFRAMEWORKBASE", "TrustFrameworkBase.xml")
$policyConfig.Add("B2C_1A_TRUSTFRAMEWORKLOCALIZATION", "TrustFrameworkLocalization.xml")
$policyConfig.Add("B2C_1A_TRUSTFRAMEWORKEXTENSIONS", "TrustFrameworkExtensions.xml")
$policyConfig.Add("B2C_1A_SIGNUP_SIGNIN", "SignUpOrSignin.xml")
#$policyConfig.Add("B2C_1A_PASSWORD_RESET", "PasswordReset.xml")
#$policyConfig.Add("B2C_1A_PROFILEEDIT", "ProfileEdit.xml")

# gets the existing policy ids
function Get-AADB2CPolicyIds { Get-AzureADMSTrustFrameworkPolicy | % { $_.Id } }

# connect to the tenant
try {
	$deployedPolicyIds = Get-AADB2CPolicyIds
}
catch [Microsoft.Open.Azure.AD.CommonLibrary.AadNeedAuthenticationException] {
	try {
		Connect-AzureAD -Tenant "eonid.onmicrosoft.com"
	}
	catch [Microsoft.Open.Azure.AD.CommonLibrary.AadAuthenticationFailedException] {
		Write-Output "auth failed."
		exit
	}
	catch {
		Write-Output "An unknown error occured during auth... exiting."
		exit
	}
	
	$deployedPolicyIds = Get-AADB2CPolicyIds
}
catch {
	Write-Output "An unknown error occured when gathering policy ids... exiting."
	exit
}

# delete any policies that are not in the deployment map
foreach($existingId in $deployedPolicyIds) {
	if (!$policyConfig.ContainsKey($existingId)) {
		Remove-AzureADMSTrustFrameworkPolicy -Id $existingId
	}
}

# deploy policy files
foreach($policyId in $policyConfig.Keys) {
	$policyPath = Join-Path -Path $PSScriptRoot -ChildPath $policyConfig[$policyId]
	$outputPath = Join-Path (Join-Path $PSScriptRoot "CreatedDeployed") $policyConfig[$policyId]
	if ($policyId -in $deployedPolicyIds) {
		Set-AzureADMSTrustFrameworkPolicy -Id $policyId -InputFilePath $policyPath -OutputFilePath $outputPath
	}
	else {
		New-AzureADMSTrustFrameworkPolicy -InputFilePath $policyPath -OutputFilePath $outputPath
	}
}