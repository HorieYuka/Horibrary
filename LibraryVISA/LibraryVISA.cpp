#include <iostream>
#include <stdio.h>
#include <stdlib.h>

#include "visa.h"

namespace VISA
{
    static char instrDescriptor[VI_FIND_BUFLEN];
    ViStatus Status;
    ViFindList DeviceList;
    ViUInt32 DeviceNumber;

    ViStatus VISA::FindResource()
    {
        ViStatus Status;

        // Open resource manager.
        Status = viOpenDefaultRM(&defaultRM);
        if (Status < VI_SUCCESS)
        {
            printf("Could not open a session to the VISA Resource Manager!\n");
            return Status;
        }

        // Find resources
        Status = viFindRsrc(defaultRM, "?*INSTR", &DeviceList, &numInstrs, instrDescriptor);
        if (Status < VI_SUCCESS)
        {
            printf("Error occured.");
            fflush(stdin);
            viClose(defaultRM);
            return Status;
        }
    }

    ViStatus VISA::OpenResource() 
    {

    }
}

static char instrDescriptor[VI_FIND_BUFLEN];
static ViUInt32 numInstrs;
static ViFindList findList;
static ViSession defaultRM, instr;
static ViStatus status;



int main(void)
{
    /* First we will need to open the default resource manager. */
    status = viOpenDefaultRM(&defaultRM);
    if (status < VI_SUCCESS)
    {
        printf("Could not open a session to the VISA Resource Manager!\n");
        exit(EXIT_FAILURE);
    }

    /*
     * Find all the VISA resources in our system and store the number of resources
     * in the system in numInstrs.  Notice the different query descriptions a
     * that are available.

        Interface         Expression
    --------------------------------------
        GPIB              "GPIB[0-9]*::?*INSTR"
        VXI               "VXI?*INSTR"
        GPIB-VXI          "GPIB-VXI?*INSTR"
        Any VXI           "?*VXI[0-9]*::?*INSTR"
        Serial            "ASRL[0-9]*::?*INSTR"
        PXI               "PXI?*INSTR"
        All instruments   "?*INSTR"
        All resources     "?*"
    */
    status = viFindRsrc(defaultRM, "?*INSTR", &findList, &numInstrs, instrDescriptor);
    if (status < VI_SUCCESS)
    {
        printf("An error occurred while finding resources.\nHit enter to continue.");
        fflush(stdin);
        getchar();
        viClose(defaultRM);
        return status;
    }

    printf("%d instruments, serial ports, and other resources found:\n\n", numInstrs);
    printf("%s \n", instrDescriptor);

    /* Now we will open a session to the instrument we just found. */
    status = viOpen(defaultRM, instrDescriptor, VI_NULL, VI_NULL, &instr);
    if (status < VI_SUCCESS)
    {
        printf("An error occurred opening a session to %s\n", instrDescriptor);
    }
    else
    {
        /* Now close the session we just opened.                            */
        /* In actuality, we would probably use an attribute to determine    */
        /* if this is the instrument we are looking for.                    */
        viClose(instr);
    }

    while (--numInstrs)
    {
        /* stay in this loop until we find all instruments */
        status = viFindNext(findList, instrDescriptor);  /* find next desriptor */
        if (status < VI_SUCCESS)
        {   /* did we find the next resource? */
            printf("An error occurred finding the next resource.\nHit enter to continue.");
            fflush(stdin);
            getchar();
            viClose(defaultRM);
            return status;
        }
        printf("%s \n", instrDescriptor);

        /* Now we will open a session to the instrument we just found */
        status = viOpen(defaultRM, instrDescriptor, VI_NULL, VI_NULL, &instr);
        if (status < VI_SUCCESS)
        {
            printf("An error occurred opening a session to %s\n", instrDescriptor);
        }
        else
        {
            /* Now close the session we just opened.                            */
            /* In actuality, we would probably use an attribute to determine    */
            /* if this is the instrument we are looking for.                    */
            viClose(instr);
        }
    }    /* end while */

    status = viClose(findList);
    status = viClose(defaultRM);
    printf("\nHit enter to continue.");
    fflush(stdin);
    getchar();

    return 0;
}
