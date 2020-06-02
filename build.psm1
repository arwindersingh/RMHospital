# TODO: Break the functionality here up:
#   - Build
#       - BuildAngular
#           - BuildDev
#           - BuildProd
#       - BuildDotnet (Prod)
#   - Run
#       - RunAngular
#           - RunAngularDev
#           - RunAngularProd
#       - RunDotnet
#           - RunDotnetDev
#           - RunDotnetProd

$WEB_CONFIG_DIR = "$pwd/web_configs/node_local_web_server"
$WEB_CONFIG = ".local-web-server.json"

$PROD_DIR = "$pwd/prod"

$SERVER_DIR = "$pwd/dotnet-server/HospitalAllocation"
$CLIENT_DIR = "$pwd/angular-client"

function Start-Build
{
    param(
        [switch]$ForProduction
    )

    # Setup dotnet server
    pushd ./dotnet-server/HospitalAllocation
    dotnet restore
    popd

    # Make sure all needed angular packages are installed
    pushd ./angular-client
    npm install
    popd

    if ($ForProduction)
    {
        mkdir -Path $PROD_DIR/public
        mkdir -Path $PROD_DIR/logs

        pushd $CLIENT_DIR
        ng build --output-path $PROD_DIR/public
        popd

        pushd $SERVER_DIR
        dotnet publish --output $PROD_DIR
        popd
    }
}

function Start-Application
{
    param(
        [switch]$InstallNodeDeps,
        [switch]$NoRebuild
    )

    if ($InstallNodeDeps)
    {
        npm install -g @angular/cli
        npm install -g local-web-server
    }

    if (-not $NoRebuild)
    {
        Start-Build
    }

    try
    {
        $clientJob = Start-Job -ArgumentList $CLIENT_DIR -ScriptBlock {
            cd $args[0]
            ng serve --port 5002
        }

        $serverJob = Start-Job -ArgumentList $SERVER_DIR -ScriptBlock {
            cd $args[0]
            dotnet run
        }

        cp $WEB_CONFIG_DIR/dev_config.json ./$WEB_CONFIG
        ws
    }
    finally
    {
        $clientJob,$serverJob | % { Stop-Job $_; Remove-Job $_ }
    }
}
