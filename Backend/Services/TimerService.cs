using Backend.Models;
using Backend.Repository;
using Backend.Services.Hub;
using Common.DTO;
using Microsoft.AspNetCore.SignalR;
using Timer = Backend.Models.Timer;

namespace Backend.Services
{
    public class TimerService
    {
        private readonly TimerRepository _repository;
        private readonly IHubContext<TimerHub, ITimerHub> _hubContext;
        public TimerService(TimerRepository repository, IHubContext<TimerHub, ITimerHub> hubContext)
        {
            _repository = repository;
            _hubContext= hubContext;
        }

        public async Task<TimerDTO> Create(TimerDTO model)
        {
            var entity = new Timer();
            entity.StartTime=model.StartTime;
            entity.EndTime=model.EndTime;
            var result=await _repository.Create(entity);
            model.Id=result.Id;
            await _hubContext.Clients.All.SendNewTimer(model);
            return model;
        }

        public async Task<List<TimerDTO>> GetAll()
        {
            var result = await _repository.GetAllTimer();
            return result;
        }
    }
}
