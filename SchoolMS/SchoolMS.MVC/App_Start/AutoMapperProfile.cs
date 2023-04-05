﻿using AutoMapper;
using SchoolMS.Model;
using SchoolMS.MVC.Models;
using SchoolMS.MVC.Models.StudentView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMS.MVC.App_Start
{
    public class AutoMapperProfile : Profile
    {       
        public AutoMapperProfile()
        {
            CreateMap<StudentEditView, StudentModelDTO>();
            CreateMap<StudentCreateView, StudentModelDTO>();
            
        }
    }
}
