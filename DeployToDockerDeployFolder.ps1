# Feature Ordercloud
Copy-Item ".\src\Feature\Ordercloud\platform\App_Config\" -Destination ".\docker\deploy\platform" -Recurse -Force

# Foundation Ordercloud
Copy-Item ".\src\Foundation\Ordercloud\platform\App_Config\" -Destination ".\docker\deploy\platform" -Recurse -Force

# Foundation Ordercloud Custom
Copy-Item ".\src\Foundation\Ordercloud Customer\platform\App_Config\" -Destination ".\docker\deploy\platform" -Recurse -Force

# Bin folder
Copy-Item ".\src\Platform\bin" -Destination ".\docker\deploy\platform\bin" -Force

# Project DLLS
Copy-Item ".\src\Platform\bin\BasicCompany.Feature.Ordercloud.dll" -Destination ".\docker\deploy\platform\bin\BasicCompany.Feature.Ordercloud.dll" -Force
Copy-Item ".\src\Platform\bin\BasicCompany.Feature.Products.dll" -Destination ".\docker\deploy\platform\bin\BasicCompany.Feature.Products.dll" -Force
Copy-Item ".\src\Platform\bin\BasicCompany.Foundation.Products.Ordercloud.Customer.dll" -Destination ".\docker\deploy\platform\bin\BasicCompany.Foundation.Products.Ordercloud.Customer.dll" -Force
Copy-Item ".\src\Platform\bin\BasicCompany.Foundation.Products.Ordercloud.dll" -Destination ".\docker\deploy\platform\bin\BasicCompany.Foundation.Products.Ordercloud.dll" -Force
Copy-Item ".\src\Platform\bin\BasicCompany.Foundation.Products.Platform.dll" -Destination ".\docker\deploy\platform\bin\BasicCompany.Foundation.Products.Platform.dll" -Force
Copy-Item ".\src\Platform\bin\Flurl.dll" -Destination ".\docker\deploy\platform\bin\Flurl.dll" -Force
Copy-Item ".\src\Platform\bin\Flurl.Http.dll" -Destination ".\docker\deploy\platform\bin\Flurl.Http.dll" -Force
Copy-Item ".\src\Platform\bin\JSS18Demo.dll" -Destination ".\docker\deploy\platform\bin\JSS18Demo.dll" -Force
Copy-Item ".\src\Platform\bin\Newtonsoft.Json.dll" -Destination ".\docker\deploy\platform\bin\Newtonsoft.Json.dll" -Force
Copy-Item ".\src\Platform\bin\OrderCloud.SDK.dll" -Destination ".\docker\deploy\platform\bin\OrderCloud.SDK.dll" -Force

# Special Case Web.config of system
Copy-Item ".\docker\WebConfigBackupFromContainer\Web.config" -Destination ".\docker\deploy\platform" -Force

# Views Deployment Feature.Ordercloud
Copy-Item ".\src\Feature\Ordercloud\platform\Views" -Destination ".\docker\deploy\platform" -Recurse -Force