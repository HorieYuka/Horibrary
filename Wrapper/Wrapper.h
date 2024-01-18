#pragma once

#include "C:\repo\HorieYuka\Horibrary\LibraryVISA\LibraryVISA.h"
#include "C:\repo\HorieYuka\Horibrary\LibraryVISA\LibraryVISA.cpp"

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
        unsigned char* IORead(int DeviceIdx);

		Library* Lib;
	};
}
