﻿using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Principal;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rema.DbAccess;
using Rema.WebApi.Filter;
using Microsoft.Extensions.Configuration;
using Rema.ServiceLayer.Services;

namespace Rema.WebApi.Controllers
{
  [Produces("application/json")]
  [Route("[controller]")]
  [ApiController]
  public class DebugController : BaseController
  {
    public DebugController(RpDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    [Route("CurrentUserMembers")]
    [HttpGet]
    public IEnumerable<string> GetCurrentUserMembers()
    {
      var securityMembers = new List<string>();
      var user = this.HttpContext.User;
      var identity = (WindowsIdentity)user.Identity;

      securityMembers.Add(identity.Name);
      foreach (var group in identity.Groups)
      {
        string groupTitle = group.Translate(typeof(NTAccount)).ToString();
        securityMembers.Add(groupTitle);
      }

      return securityMembers;
    }

    [HttpGet("UserMembers/{AdName}")]
    [AuthorizeAd("Admin")]
    public IEnumerable<string> GetUserMembers(string AdName)
    {
      var adService = new AdService();
      var securityMembers = new List<string>();

      if (!AdName.Contains("@"))
      {
        securityMembers.Add("Bitte geben Sie den Active-Directory Name im Format 'name@domäne' an!");
        return securityMembers;
      }
      string username = AdName.Split('@')[0].ToString();
      string domainName = AdName.Split('@')[1].ToString();

      Domain domain = adService.FindDomainByName(domainName);
      if (null == domain)
      {
        securityMembers.Add(string.Format("Es konnte keine Domäne zu '{0}' ermittelt werden!", domainName));
        return securityMembers;
      }

      securityMembers = adService.GetMemberListByUserName(domain.Name, username);

      return securityMembers;
    }

    [HttpGet("adUser")]
    public IEnumerable<string> GetAllAdUsers()
    {
      var adService = new AdService(Startup.DomainsToSearch);

      List<string> adUsers = adService.SearchAdUsers("");

      return adUsers;
    }

    [HttpGet("adUser/{namePart}")]
    public IEnumerable<string> GetAdUsers(string namePart)
    {
      var adService = new AdService(Startup.DomainsToSearch);

      List<string> adUsers = adService.SearchAdUsers(namePart);

      return adUsers;
    }
  }
}
