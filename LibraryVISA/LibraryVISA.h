#pragma once

#include "visa.h"

namespace VISA
{
    static class LibraryVISA
    {
    public:
        ViStatus Status;
        ViFindList DeviceList;
    private:
        float m_XPos, m_YPos;
    public:
        ViStatus FindResource();


    };
}