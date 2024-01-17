#pragma once

#include <stdio.h>

#include "visa.h"
#

class Library
{
public:
    char** GetDescList();
    bool FindResource();
    bool OpenSession(int Idx);
    bool CloseSession(int Idx);
    bool DisposeManager();
};