using ANF.Core;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using ANF.Infrastructure;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Service
{
    public class PolicyService(IUnitOfWork unitOfWork, IMapper mapper) : IPolicyService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork; 
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CreatePolicy(PolicyCreateRequest request)
        {
            try
            {
                request.Header = CleanAndTrim(request.Header);
                request.Description = CleanAndTrim(request.Description);

                var policyRepo = _unitOfWork.GetRepository<Policy>();
                if (request != null)
                {
                    //check duplicate by header
                    var existedPolicy = await policyRepo.GetAll()
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(p => p.Header == request.Header);
                    if (existedPolicy != null) { throw new DuplicatedException("policy \"" + request.Header + "\" already in database"); }

                    var policy = _mapper.Map<Policy>(request);
                    policy.CreatedAt = DateTime.UtcNow;
                    policyRepo.Add(policy);
                    return await _unitOfWork.SaveAsync() > 0;
                }
                else
                {
                    throw new ArgumentException("Invalid request.");
                    //return false;
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
                //return false;
            }
        }

        public async Task<bool> DeletePolicy(long id)
        {
            try
            {
                var policyRepo = _unitOfWork.GetRepository<Policy>();
                var existedPolicy = await policyRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                
                if (existedPolicy is null)
                {
                    throw new KeyNotFoundException("Not found Policy with id=" + id);
                }

                policyRepo.Delete(existedPolicy);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginationResponse<PolicyResponse>> GetPolicies(PaginationRequest request)
        {
            var policyRepo = _unitOfWork.GetRepository<Policy>();
            var policies = await policyRepo.GetAll()
                                    .AsNoTracking()
                                    .Skip((request.pageNumber - 1)* request.pageSize)
                                    .Take(request.pageSize)
                                    .ToListAsync();
            if (!policies.Any()) throw new KeyNotFoundException("No Policy found.");
            var totalCount = policies.Count();

            var response = _mapper.Map<List<PolicyResponse>>(policies);
            return new PaginationResponse<PolicyResponse>(response, totalCount, request.pageNumber, request.pageSize);
        }

        public async Task<PolicyResponse> GetPolicyById(long policyId)
        {
            var policyRepo = _unitOfWork.GetRepository<Policy>();
            var policy = await policyRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == policyId);

            if (policy is null) throw new KeyNotFoundException("not found policy with Id:" + policyId);

            var response = _mapper.Map<PolicyResponse>(policy);
            return response;

        }

        public async Task<bool> UpdatePolicy(long id, PolicyUpdateRequest request)
        {
            try {
                var policyRepo = _unitOfWork.GetRepository<Policy>();
                var existedPolicy = await policyRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                //check id
                if (existedPolicy is null) { throw new KeyNotFoundException("Not found Policy with id=" + id); }

                //check duplicate by header
                var existedHeaderPolicy = await policyRepo.GetAll()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(p => p.Header == request.Header);
                if (existedHeaderPolicy != null) { throw new DuplicatedException("policy \"" + request.Header + "\" already in database"); }

                request.Header = CleanAndTrim(request.Header);
                request.Description = CleanAndTrim(request.Description);

                _mapper.Map(request, existedPolicy);
                policyRepo.Update(existedPolicy);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception e){
                await _unitOfWork.RollbackAsync();
                throw new Exception(e.Message);
            }
        }
        private string CleanAndTrim(string? str)
        {
            return string.IsNullOrWhiteSpace(str)
                ? ""
                : string.Join(" ", str.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
