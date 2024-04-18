using Common.DTO;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Services.Hub
{
    public class TimerHub: Hub<ITimerHub>
    {
    }
    public interface ITimerHub
    {
        Task SendNewTimer(TimerDTO data);
    }
}
