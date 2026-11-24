using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.Category.Queries.Models;
using EduPro.Core.Features.Category.Queries.Results;
using EduPro.Core.Resources;
using EduPro.Service.Abstracts;
using MediatR;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Category.Queries.Handlers
{
    public class CategoryQueryHandler : ResponseHandler,
          IRequestHandler<GetCategoryListQuery, Response<List<GetCategoryListResponse>>>,
          IRequestHandler<GetCategoryByIdQuery, Response<GetCategoryListResponse>>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CategoryQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                    ICategoryService categoryService,
                                    IMapper mapper) : base(stringLocalizer)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<List<GetCategoryListResponse>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetCategoryListAsync();
            var result = _mapper.Map<List<GetCategoryListResponse>>(categories);
            return Success(result);
        }

        public async Task<Response<GetCategoryListResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(request.Id);
            if (category == null)
                return NotFound<GetCategoryListResponse>(_localizer[SharedResourcesKeys.NotFound]);
            var result = _mapper.Map<GetCategoryListResponse>(category);
            return Success(result);
        }
        #endregion
    }
}