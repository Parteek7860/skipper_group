using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Repositories
{
    public interface IAudioRepo
    {
        Task<int> AddAudioVideo(AudioVideoModel model);
        Task<DataTable> GetAudioVideoList();
        Task<int> DeleteAudioVideo(int id);
        Task<int> ChangeStatus(int id);
    }
}
