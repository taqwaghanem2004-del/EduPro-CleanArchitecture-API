using EduPro.Core.Features.EduFile.Commands.Models;
using EduPro.Data.Enums;
using EduPro.Service.Abstracts;
using FluentValidation;

namespace EduPro.Core.Features.EduFile.Commands.Validators
{
    public class UploadEduFileValidator : AbstractValidator<UploadEduFileCommand>
    {
        private readonly ICourseService _courseService;

        public UploadEduFileValidator(ICourseService courseService)
        {
            _courseService = courseService;
            ApplyValidationsRules();
        }

        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(300).WithMessage("Title must not exceed 300 characters");

            RuleFor(x => x.FileType)
                .IsInEnum().WithMessage("Invalid file type");

            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                {
                    if (command.CourseId.HasValue)
                        return await _courseService.GetCourseByIdAsync(command.CourseId.Value) != null;

                    if (!string.IsNullOrEmpty(command.RequestedCourseName))
                        return true;

                    return false;
                })
                .WithMessage("يجب تحديد كورس موجود أو إدخال اسم كورس جديد");

            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required")
                .Must(file => file != null && file.Length > 0).WithMessage("File is empty")
                .Must(file => file != null && file.Length <= 500 * 1024 * 1024)
                .WithMessage("File size must not exceed 500MB")
                .Must((command, file) =>
                {
                    if (command.FileType == EduFileType.Lecture)
                    {
                        var ext = Path.GetExtension(file.FileName).ToLower();
                        return ext == ".mp4" || ext == ".avi" || ext == ".mov" || ext == ".mkv";
                    }
                    return true;
                })
                .WithMessage("Lecture must be MP4, AVI, MOV or MKV format");
        }
    }
}