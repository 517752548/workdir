@echo off
pushd ToolBin
ExcelOut dir:../ExcelConfig namespace:ExcelConfig  exportJson:../Unity/Assets/StreamingAssets/Json exportCs:../src/NetSources/Config.cs ex:*.xlsx

ProtoParser dir:../Net file:const.proto saveto:../src/NetSources/GameConst.cs
ProtoParser dir:../Net file:data.proto saveto:../src/NetSources/GameData.cs
ProtoParser dir:../Net file:Message.proto saveto:../src/NetSources/GameMessage.cs
popd
C:\WINDOWS\Microsoft.NET\Framework\v3.5\csc /target:library /out:%cd%\Unity\Assets\Plugins\proto.dll %cd%\src\NetSources\*.cs

Copy %cd%\Unity\Assets\StreamingAssets\Json\*.json %cd%\Server\Configs  /Y
copy %cd%\Unity\Assets\Plugins\proto.dll %cd%\Server\Lib\  /Y
pause