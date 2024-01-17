#pragma once

#include "C:\repo\HorieYuka\Horibrary\LibraryVISA\LibraryVISA.h"
#include "C:\repo\HorieYuka\Horibrary\LibraryVISA\LibraryVISA.cpp"

namespace Wrapper {
	public ref class LibraryVISA
	{
	public:
		LibraryVISA();

		char** GetDescList();
		bool FindResource();
		bool OpenSession(int Idx);
		bool CloseSession(int Idx);
		bool DisposeManager();

		Library* Lib;
	};
}
