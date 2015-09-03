@echo off
pushd ToolBin
ExcelOut dir:../ExcelConfig namespace:ExcelConfig  exportJson:../Unity/Assets/StreamingAssets/Json exportCs:../src/NetSources/Config.cs ex:*.xlsx mode:nocode
popd
Copy %cd%\Unity\Assets\StreamingAssets\Json\*.json %cd%\Server\Configs  /Y
pause