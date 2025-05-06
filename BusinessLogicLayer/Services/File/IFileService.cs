namespace BusinessLogicLayer.Services.File;
using Microsoft.AspNetCore.Http;

public interface IFileService
{
    Task<string> UploadFile(IFormFile file, string destinationFolder);
}