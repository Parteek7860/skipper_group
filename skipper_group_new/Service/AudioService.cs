using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using System.Data;

namespace skipper_group_new.Service
{
    public class AudioService : IAudioSer
    {
        private readonly IAudioRepo _audioRepo;
        public AudioService(IAudioRepo audioRepo)
        {
            _audioRepo = audioRepo;
        }
        public Task<int> AddAudioVideo(AudioVideoModel model) => _audioRepo.AddAudioVideo(model);
        public Task<DataTable> GetAudioVideoList() => _audioRepo.GetAudioVideoList();
        public Task<int> DeleteAudioVideo(int id) => _audioRepo.DeleteAudioVideo(id);
        public Task<int> ChangeStatus(int id)=> _audioRepo.ChangeStatus(id);
    }
}
