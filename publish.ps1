# Requires dotnet-warp installed
# Requires rimraf installed too

rimraf ./dist

dotnet warp -r win-x64 ./src
mkdir ./dist/x64
Move-Item Pinger.exe ./dist/x64

dotnet warp -r win-x86 ./src
mkdir ./dist/x86
Move-Item Pinger.exe ./dist/x86

dotnet warp -r osx-x64 ./src
mkdir ./dist/osx
Move-Item Pinger ./dist/osx

dotnet warp -r linux-x64 ./src
mkdir ./dist/linux
Move-Item Pinger ./dist/linux
