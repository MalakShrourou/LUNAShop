using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class CompanyInfoService
    {
        private readonly ICompanyInfoRepository _repository;
        private readonly IImageService _imageService;

        public CompanyInfoService(ICompanyInfoRepository repository, IImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
        }

        public async Task UpdateCompanyInfo(CompanyInfoCommand command)
        {

            CompanyInfo companyInfo = await _repository.Get();
            companyInfo.UpdatedOn = DateTime.Now;
            companyInfo.AddressEN=command.AddressEN;
            companyInfo.AddressAR=command.AddressAR;
            companyInfo.Facebook=command.Facebook;
            companyInfo.X=command.X;
            companyInfo.OverviewEN=command.OverviewEN;
            companyInfo.OverviewAR=command.OverviewAR;
            companyInfo.Email=command.Email;
            companyInfo.Phone=command.Phone;
            companyInfo.ValuesAR=command.ValuesAR;
            companyInfo.ValuesEN=command.ValuesEN;
            companyInfo.InvolvementEN=command.InvolvementEN;
            companyInfo.InvolvementAR=command.InvolvementAR;
            companyInfo.Name=command.Name;
            companyInfo.Instagram=command.Instagram;
            companyInfo.LinkedIn=command.LinkedIn;
            if(command.Logo != null)
            {
                string logo = await _imageService.SaveImage(command.Logo);
                companyInfo.Logo = logo;

            }
            await _repository.Update(companyInfo);
        }

        public async Task<CompanyInfoModel> GetCompanyInfo()
        {
            CompanyInfo companyInfo = await _repository.Get();
            return new CompanyInfoModel
            {
                Id = companyInfo.Id,
                AddressAR = companyInfo.AddressAR,
                AddressEN = companyInfo.AddressEN,
                Email = companyInfo.Email,
                Facebook = companyInfo.Facebook,
                Instagram = companyInfo.Instagram,
                InvolvementAR = companyInfo.InvolvementAR,
                InvolvementEN = companyInfo.InvolvementEN,
                LinkedIn = companyInfo.LinkedIn,
                Name = companyInfo.Name,
                OverviewAR = companyInfo.OverviewAR,
                OverviewEN = companyInfo.OverviewEN,
                Phone = companyInfo.Phone,
                ValuesAR = companyInfo.ValuesAR,
                ValuesEN = companyInfo.ValuesEN,
                X = companyInfo.X,
                Logo = await _imageService.GetImageBase64(companyInfo.Logo),
                UpdatedOn = companyInfo.UpdatedOn,
                CreatedOn = companyInfo.CreatedOn
            };
        }
    }
}
