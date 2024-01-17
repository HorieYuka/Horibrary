#pragma once

#include "C:\Repos\Horibrary\LibraryVISA\LibraryVISA.h"
#include "C:\Repos\Horibrary\LibraryVISA\LibraryVISA.cpp"

namespace Wrapper {
	public ref class LibraryVISA
	{
	public:
		LibraryVISA();

        int GetDeviceCount();
        char* GetDeviceInfo(int DeviceIdx);

        bool FindResource();
        bool OpenSession(int DeviceIdx);
        bool CloseSession(int DeviceIdx);
        bool DisposeManager();

        bool IOWrite(int DeviceIdx, char* Str, unsigned int StrLen);
        unsigned char* IORead(int DeviceIdx, char* Str, unsigned int StrLen);

		Library* Lib;
	};
}
