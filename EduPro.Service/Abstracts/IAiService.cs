namespace EduPro.Service.Abstracts
{
    public interface IAiService
    {
        Task<string> UploadAndProcessFileAsync(int fileId, string absoluteFilePath, string fileName);
        Task<string> AskQuestionAsync(int fileId, string question);
    }
}