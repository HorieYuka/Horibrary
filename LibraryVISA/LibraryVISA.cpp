#pragma once

#include "LibraryVISA.h"
#include "visa.h"

const int DeviceLimit = 20;

ViFindList FindList;
ViSession ResourceManager;

ViSession SessionList[DeviceLimit];
char InstDescriptor[DeviceLimit][VI_FIND_BUFLEN];

char** Library::GetDescList()
{
    char** Out = new char* [DeviceLimit];
    for (int i = 0; i < DeviceLimit; i++) {
        Out[i] = new char[VI_FIND_BUFLEN];
        
        for (int j = 0; j < VI_FIND_BUFLEN; j++)
            Out[i] = &InstDescriptor[i][j];
    }

    return Out;
}

bool Library::FindResource()
{
    ViStatus Status;

    ViUInt32 DeviceCount;
    ViUInt32 Inst;

    char Dummy[VI_FIND_BUFLEN];

    // Open resource manager.
    Status = viOpenDefaultRM(&ResourceManager);
    if (Status < VI_SUCCESS)
    {
        printf("Cannot open VISA Resource manager\n");
        return false;
    }

    // Find resources
    Status = viFindRsrc(ResourceManager, "?*INSTR", &FindList, &DeviceCount, Dummy);
    if (Status < VI_SUCCESS)
    {
        printf("Error occured.");
        fflush(stdin);
        viClose(ResourceManager);
        return false;
    }

    // Find all instrument
    while (--DeviceCount)
    {
        Status = viFindNext(FindList, InstDescriptor[DeviceCount - 1]);
        if (Status < VI_SUCCESS)
            return false;

        OpenSession(DeviceCount - 1);
        CloseSession(DeviceCount - 1);
    }

    return true;
}

bool Library::OpenSession(int Idx)
{
    ViStatus Status;

    Status = viOpen(ResourceManager, InstDescriptor[Idx], VI_NULL, VI_NULL, &SessionList[Idx]);
    if (Status < VI_SUCCESS)
    {
        printf("Could not open a session\n");
        return false;
    }


    return true;
}

bool Library::CloseSession(int Idx)
{
    ViStatus Status;

    Status = viClose(SessionList[Idx]);
    if (Status < VI_SUCCESS)
    {
        printf("Could not close a session\n");
        return false;
    }

    return true;
}


bool Library::DisposeManager()
{
    ViStatus Status;

    Status = viClose(FindList);
    Status = viClose(ResourceManager);
    fflush(stdin);

    if (Status < VI_SUCCESS)
    {
        printf("Could not dispose\n");
        return false;
    }

    return true;
}


