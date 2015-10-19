================================================================================================================================================================================
CoreMvc - Installation Instructions
================================================================================================================================================================================

1. In <appSettings>, ensure that you have the following elements and give them valid values if you are using these features:
	IpAdressAuthorizeAttribute:
	    <add key="CoreMvc.AuthorizedIpAddresses" value="111.1.1.1|222.2.2.2" />
	BasicAuthorizeAttribute:
	    <add key="CoreMvc.AdminUsername" value="admin" />
		<add key="CoreMvc.AdminPassword" value="some-secret-password" />