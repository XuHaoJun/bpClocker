# Notes

## Create Project

```sh
dotnet new sln -n bpClocker
mkdir src
cd src
func init --docker BpClockerAzureFunction --worker-runtime dotnet-isolated --target-framework net8.0
cd ..
dotnet sln bpClocker.sln add src/BpClockerAzureFunction
```

## Problems

- Function apps are not supported in Free or Shared App Service Plans. (代碼: FreeOrSharedFunctionsAppServicePlanNotSupported)

## Reference

- <https://learn.microsoft.com/zh-tw/dotnet/core/tools/dotnet-sln>
