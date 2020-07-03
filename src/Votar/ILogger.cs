using System;

namespace Votar
{
    public interface ILogger
    {
        void LogError(Exception ex);
    }
}