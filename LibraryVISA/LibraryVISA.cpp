#pragma once

#include "LibraryVISA.h"
#include "visa.h"

const int DeviceLimit = 20;
unsigned int DeviceCount;

ViFindList FindList;
ViSession ResourceManager;

ViSession SessionList[DeviceLimit];

bool bIsSessOpened[DeviceLimit];
char InstDescriptor[DeviceLimit][VI_FIND_BUFLEN];


char* Library::GetDeviceInfo(int Idx)
{
    char* Out = new char[VI_FIND_BUFLEN];

    Out = InstDescriptor[Idx];
        
    return Out;
}

int Library::GetDeviceCount()
{
    return DeviceCount;
}

bool Library::FindResource()
{
    ViStatus Status;

    ViUInt32 Count;
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
    Status = viFindRsrc(ResourceManager, "?*INSTR", &FindList, &Count, Dummy);
    if (Status < VI_SUCCESS)
    {
        printf("Error occured.");
        fflush(stdin);
        viClose(ResourceManager);
        return false;
    }

    DeviceCount = Count - 1;

    // Find all instrument
    while (--Count)
    {
        /* stay in this loop until we find all instruments */
        Status = viFindNext(FindList, Dummy);  /* find next desriptor */
        if (Status < VI_SUCCESS)
        {   /* did we find the next resource? */
            printf("An error occurred finding the next resource.\nHit enter to continue.");
            fflush(stdin);
            return false;
        }

        for (int i = 0; i < VI_FIND_BUFLEN; i++)
            InstDescriptor[Count - 1][i] = Dummy[i];

        OpenSession(Count - 1);
        CloseSession(Count - 1);
    }

    return true;
}

bool Library::OpenSession(int DeviceIdx)
{
    ViStatus Status;

    Status = viOpen(ResourceManager, InstDescriptor[DeviceIdx], VI_NULL, VI_NULL, &SessionList[DeviceIdx]);
    if (Status < VI_SUCCESS)
    {
        printf("Could not open a session\n");
        return false;
    }

    bIsSessOpened[DeviceIdx] = true;
    return true;
}

bool Library::CloseSession(int DeviceIdx)
{
    ViStatus Status;

    Status = viClose(SessionList[DeviceIdx]);
    if (Status < VI_SUCCESS)
    {
        printf("Could not close a session\n");
        return false;
    }

    bIsSessOpened[DeviceIdx] = false;

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

bool Library::IOWrite(int DeviceIdx, char* Str, unsigned int StrLen)
{
    ViStatus Status;
    ViUInt32 Writecount; // It referrs actual write length. Considering it as a dummy.

    if (!bIsSessOpened[DeviceIdx])
        return false;
    else
    {
        Status = viWrite(SessionList[DeviceIdx], (ViBuf) Str, (ViUInt32)StrLen, &Writecount);
        if (Status < VI_SUCCESS)
        {
            printf("Error writing to the device\n");
            return false;
        }
    }

    return true;
}

unsigned char* Library::IORead(int DeviceIdx)
{
    ViStatus Status;
    ViUInt32 RetCount; // It referrs actual read length. Considering it as a dummy.

    unsigned char Out[VI_FIND_BUFLEN / 2];

    if (!bIsSessOpened[DeviceIdx])
        return Out;
    else
    {
        Status = viRead(SessionList[DeviceIdx], Out, VI_FIND_BUFLEN / 2, &RetCount);
        if (Status < VI_SUCCESS)
        {
            printf("Error writing to the device\n");
        }
    }

    return Out;
}

