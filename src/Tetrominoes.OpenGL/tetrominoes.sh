#!/bin/bash

DOTNET_PATH=$(which dotnet)
if [ $? -ne 0 ]; then
  echo "dotnet is not currently installed. See https://dot.net";
  exit 1;
fi

SCRIPT=`realpath -s $0`
SCRIPTPATH=`dirname $SCRIPT`

pushd $SCRIPTPATH
dotnet ./Tetrominoes.OpenGL.dll
popd
