﻿using BlogService.Common.Dtos.General;

namespace BlogService.Services.Interfaces
{
    public interface IFilesService
    {
        Task<string> SaveFile(string path, IFormFile file, string? name = null);
        Task<FileDto?> GetFile(string path, string fileName);
    }
}
