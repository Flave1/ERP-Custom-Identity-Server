using APIGateway.Contracts.Response.HRM;
using APIGateway.Data;
using APIGateway.Handlers.Hrm.setup.jobdetail;
using APIGateway.Handlers.Hrm.setup.jobgrade;
using APIGateway.Handlers.Hrm.setup.languages;
using APIGateway.Handlers.Hrm.setup.proffesional_membership;
using APIGateway.Handlers.Hrm.setup.proffessional_certification;
using GODPAPIs.Contracts.V1;
using MediatR;
using Microsoft.AspNetCore.Mvc; 
using System.Threading.Tasks;

namespace APIGateway.Controllers.V1.hrm
{
    public class Hrm_setupController : Controller
    {
        private readonly IMediator _mediator;
        public Hrm_setupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region academic_discipline 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_ACADEMIC_DISCPLINE)]
        public async Task<IActionResult> ADD_UPDATE_ACADEMIC_DISCPLINE([FromBody] hrm_setup_academic_discipline_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_ACADEMIC_DISCPLINE)]
        public async Task<IActionResult> DELETE_ACADEMIC_DISCPLINE([FromBody] Deletehrm_setup_academic_discipline command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_ACADEMIC_DISCPLINE)]
        public async Task<IActionResult> GET_ALL_ACADEMIC_DISCPLINE()
        {
            var query = new GetAll_hrm_setup_academic_discipline_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_ACADEMIC_DISCPLINE)]
        public async Task<IActionResult> GET_SINGLE_ACADEMIC_DISCPLINE([FromQuery] GetSingle_hrm_setup_academic_discipline_Query query)
        {
            return Ok(await _mediator.Send(query));
        }




        #endregion

        #region academic_grade 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_ACADEMIC_GRADES)]
        public async Task<IActionResult> ADD_UPDATE_ACADEMIC_GRADES([FromBody] hrm_setup_academic_grade_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_ACADEMIC_GRADES)]
        public async Task<IActionResult> DELETE_ACADEMIC_GRADES([FromBody] Deletehrm_setup_academic_grade command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_ACADEMIC_GRADES)]
        public async Task<IActionResult> GET_ALL_ACADEMIC_GRADES()
        {
            var query = new GetAll_hrm_setup_academic_grade_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_ACADEMIC_GRADES)]
        public async Task<IActionResult> GET_SINGLE_ACADEMIC_GRADES([FromQuery] GetSingle_hrm_setup_academic_grade_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion

        #region academic_qualification 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_ACADEMIC_QUALIFICATION)]
        public async Task<IActionResult> ADD_UPDATE_ACADEMIC_QUALIFICATION([FromBody] hrm_setup_academic_qualification_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_ACADEMIC_QUALIFICATION)]
        public async Task<IActionResult> DELETE_ACADEMIC_QUALIFICATION([FromBody] Deletehrm_setup_academic_qualification command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_ACADEMIC_QUALIFICATION)]
        public async Task<IActionResult> GET_ALL_ACADEMIC_QUALIFICATION()
        {
            var query = new GetAll_hrm_setup_academic_qualification_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_ACADEMIC_QUALIFICATION)]
        public async Task<IActionResult> GET_SINGLE_ACADEMIC_QUALIFICATION([FromQuery] GetSingle_hrm_setup_academic_qualification_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion

        #region employmenttype 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_EMPLYMENT_TYPE)]
        public async Task<IActionResult> ADD_UPDATE_EMPLYMENT_TYPE([FromBody] hrm_setup_employmenttype_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_EMPLYMENT_TYPE)]
        public async Task<IActionResult> DELETE_EMPLYMENT_TYPE([FromBody] Deletehrm_setup_employmenttype command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_EMPLYMENT_TYPE)]
        public async Task<IActionResult> GET_ALL_EMPLYMENT_TYPE()
        {
            var query = new GetAll_hrm_setup_employmenttype_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_EMPLYMENT_TYPE)]
        public async Task<IActionResult> GET_SINGLE_EMPLYMENT_TYPE([FromQuery] GetSingle_hrm_setup_employmenttype_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion

        #region employmetlevel 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_EMPLYMENT_LEVEL)]
        public async Task<IActionResult> ADD_UPDATE_EMPLYMENT_LEVEL([FromBody] hrm_setup_employmentlevel_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_EMPLYMENT_LEVEL)]
        public async Task<IActionResult> DELETE_EMPLYMENT_LEVEL([FromBody] Deletehrm_setup_employmentlevel command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_EMPLYMENT_LEVEL)]
        public async Task<IActionResult> GET_ALL_EMPLYMENT_LEVEL()
        {
            var query = new GetAll_hrm_setup_employmentlevel_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_EMPLYMENT_LEVEL)]
        public async Task<IActionResult> GET_SINGLE_EMPLYMENT_LEVEL([FromQuery] GetSingle_hrm_setup_employmentlevel_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion

        #region gym_workouts 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_GYM_WORKOUT)]
        public async Task<IActionResult> ADD_UPDATE_GYM_WORKOUT([FromBody] hrm_setup_gym_workouts_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_GYM_WORKOUT)]
        public async Task<IActionResult> DELETE_GYM_WORKOUT([FromBody] Deletehrm_setup_gym_workouts command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_GYM_WORKOUT)]
        public async Task<IActionResult> GET_ALL_GYM_WORKOUT()
        {
            var query = new GetAll_hrm_setup_gym_workouts_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_GYM_WORKOUT)]
        public async Task<IActionResult> GET_SINGLE_GYM_WORKOUT([FromQuery] GetSingle_hrm_setup_gym_workouts_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion

        #region high_school_grade 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_HIGH_SCHOOL_GRADE)]
        public async Task<IActionResult> ADD_UPDATE_HIGH_SCHOOL_GRADE([FromBody] hrm_setup_high_school_grade_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_HIGH_SCHOOL_GRADE)]
        public async Task<IActionResult> DELETE_HIGH_SCHOOL_GRADE([FromBody] Deletehrm_setup_high_school_grade command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_HIGH_SCHOOL_GRADE)]
        public async Task<IActionResult> GET_ALL_HIGH_SCHOOL_GRADE()
        {
            var query = new GetAll_hrm_setup_high_school_grade_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_HIGH_SCHOOL_GRADE)]
        public async Task<IActionResult> GET_SINGLE_HIGH_SCHOOL_GRADE([FromQuery] GetSingle_hrm_setup_high_school_grade_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion


        #region high_school_subjects 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_HIGH_SCHOOL_SUBJECT)]
        public async Task<IActionResult> ADD_UPDATE_HIGH_SCHOOL_SUBJECT([FromBody] hrm_setup_high_school_subjects_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_HIGH_SCHOOL_SUBJECT)]
        public async Task<IActionResult> DELETE_HIGH_SCHOOL_SUBJECT([FromBody] Deletehrm_setup_high_school_subjects command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_HIGH_SCHOOL_SUBJECT)]
        public async Task<IActionResult> GET_ALL_HIGH_SCHOOL_SUBJECT()
        {
            var query = new GetAll_hrm_setup_high_school_subjects_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_HIGH_SCHOOL_SUBJECT)]
        public async Task<IActionResult> GET_SINGLE_HIGH_SCHOOL_SUBJECT([FromQuery] GetSingle_hrm_setup_high_school_subjects_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion

        #region hmo 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_HMO)]
        public async Task<IActionResult> ADD_UPDATE_HMO([FromBody] hrm_setup_hmo_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_HMO)]
        public async Task<IActionResult> DELETE_HMO([FromBody] Deletehrm_setup_hmo command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_HMO)]
        public async Task<IActionResult> GET_ALL_HMO()
        {
            var query = new GetAll_hrm_setup_hmo_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_HMO)]
        public async Task<IActionResult> GET_SINGLE_HMO([FromQuery] GetSingle_hrm_setup_hmo_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion

        #region jobdetails 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_JOB_DETAILS)]
        public async Task<IActionResult> ADD_UPDATE_JOB_DETAILS([FromBody] hrm_setup_jobdetails_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_JOB_DETAILS)]
        public async Task<IActionResult> DELETE_JOB_DETAILS([FromBody] Deletehrm_setup_jobetails command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_JOB_DETAILS)]
        public async Task<IActionResult> GET_ALL_JOB_DETAILS()
        {
            var query = new GetAll_hrm_setup_jobdetails_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_JOB_DETAILS)]
        public async Task<IActionResult> GET_SINGLE_JOB_DETAILS([FromQuery] GetAll_hrm_setup_jobdetails_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion


        #region job grade 
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_JOB_GRADE)]
        public async Task<IActionResult> ADD_UPDATE_JOB_GRADE([FromBody] hrm_setup_jobgrade_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_JOB_GRADE)]
        public async Task<IActionResult> DELETE_JOB_GRADE([FromBody] Deletehrm_setup_jobgrade command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_JOB_GRADE)]
        public async Task<IActionResult> GET_ALL_JOB_GRADE()
        {
            var query = new GetAll_hrm_setup_jobgrade_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_JOB_GRADE)]
        public async Task<IActionResult> GET_SINGLE_JOB_GRADE([FromQuery] GetSingle_hrm_setup_jobgrade_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion


        #region languages
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_LANGUAGE)]
        public async Task<IActionResult> ADD_UPDATE_LANGUAGE([FromBody] hrm_setup_languages_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_LANGUAGE)]
        public async Task<IActionResult> DELETE_LANGUAGE([FromBody] Deletehrm_setup_languages command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_LANGUAGE)]
        public async Task<IActionResult> GET_ALL_LANGUAGE()
        {
            var query = new GetAll_hrm_setup_languages_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_LANGUAGE)]
        public async Task<IActionResult> GET_SINGLE_LANGUAGE([FromQuery] GetSingle_hrm_setup_languages_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion

        #region professional membership
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_PROF_MEMBERSHIP)]
        public async Task<IActionResult> ADD_UPDATE_PROF_MEMBERSHIP([FromBody] hrm_setup_proffesional_membership_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_PROF_MEMBERSHIP)]
        public async Task<IActionResult> DELETE_PROF_MEMBERSHIP([FromBody] Deletehrm_setup_proffesional_membership command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_PROF_MEMBERSHIP)]
        public async Task<IActionResult> GET_ALL_PROF_MEMBERSHIP()
        {
            var query = new GetAll_hrm_setup_proffesional_membership_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_PROF_MEMBERSHIP)]
        public async Task<IActionResult> GET_SINGLE_PROF_MEMBERSHIP([FromQuery] GetSingle_hrm_setup_proffesional_membership_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion

        #region professional certification
        [HttpPost(ApiRoutes.Hrm_setup_endpoints.ADD_UPDATE_PROF_CERTIFICATION)]
        public async Task<IActionResult> ADD_UPDATE_PROF_CERTIFICATION([FromBody] hrm_setup_proffessional_certification_contract command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Hrm_setup_endpoints.DELETE_PROF_CERTIFICATION)]
        public async Task<IActionResult> DELETE_PROF_CERTIFICATION([FromBody] Deletehrm_setup_proffessional_certification command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_ALL_PROF_CERTIFICATION)]
        public async Task<IActionResult> GET_ALL_PROF_CERTIFICATION()
        {
            var query = new GetAll_hrm_setup_proffessional_certification_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Hrm_setup_endpoints.GET_SINGLE_PROF_CERTIFICATION)]
        public async Task<IActionResult> GET_SINGLE_PROF_CERTIFICATION([FromQuery] GetSingle_hrm_setup_proffessional_certification_Query query)
        {
            return Ok(await _mediator.Send(query));
        }


        #endregion
    }
}
