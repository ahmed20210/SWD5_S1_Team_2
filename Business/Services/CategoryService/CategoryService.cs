

using AutoMapper;
using Business.Services.StorageService;
using Business.ViewModels.CategoryViewModel;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Business.Services.CategoryService
{

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private readonly IStorageService _storageService;

        public CategoryService(ApplicationDbContext context, IMapper mapper, IStorageService storageService)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            
            return _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        }

        public async Task<CategoryViewModel> CreateCategoryAsync(CreateCategoryViewModel categoryViewModelForm)
        {
            // convert the image to a file stream
            using var stream = new MemoryStream();
            await categoryViewModelForm.Image.CopyToAsync(stream);
            stream.Position = 0;
            // upload the image to the storage service

            var ImageProps = await _storageService.UploadPhotoAsync(
                stream,
                categoryViewModelForm.Image.FileName,
                categoryViewModelForm.Image.ContentType
            );
            if (!ImageProps.success)
            {
                throw new Exception("Image upload failed");
            }
            categoryViewModelForm.ImagePath = ImageProps.fileUrl;

            var category = _mapper.Map<Category>(categoryViewModelForm);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryViewModel>(category);
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return null;

            return _mapper.Map<CategoryViewModel>(category);
        }

        public async Task<CategoryViewModel> UpdateCategoryAsync(UpdateCategoryViewModel categoryViewModelForm)
        {
            
            var category = await _context.Categories.FindAsync(categoryViewModelForm.Id);
            if (category == null)
                return null;


            if (categoryViewModelForm.Image != null)
            {
                using var stream = new MemoryStream();
                await categoryViewModelForm.Image.CopyToAsync(stream);
                stream.Position = 0;
                // upload the image to the storage service

                var ImageProps = await _storageService.UploadPhotoAsync(
                    stream,
                    categoryViewModelForm.Image.FileName,
                    categoryViewModelForm.Image.ContentType
                );
                if (!ImageProps.success)
                {
                    throw new Exception("Image upload failed");
                }
                categoryViewModelForm.ImagePath = ImageProps.fileUrl;
            }

            _mapper.Map(categoryViewModelForm, category);

            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryViewModel>(category);
        }


        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {

                return false;
            }
        }

    }
}
