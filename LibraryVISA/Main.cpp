// Test library

#include "LibraryVISA.h"

char* InstDescriptor;

int main() 
{
	Library* Lib = new Library();
	Lib->FindResource();
	InstDescriptor = Lib->GetDeviceInfo(1);

	return 0;
}