using Microsoft.AspNetCore.Mvc;
using Mock_AWSRDS_API.Interface;
using Mock_AWS_API.Repos;
using Mock_AWS_API.Services;
using Mock_AWSRDS_API.Models;

namespace Mock_AWS_API.Controllers;

[ApiController]
[Route("/api/v1/mock")]
public class ShinobiController : ControllerBase
{
    private readonly IMockService _mockService;
    private readonly ILogger<ShinobiController> _logger;
    
    public ShinobiController(IMockService mockService, ILogger<ShinobiController> logger)
    {        
        _mockService = mockService;
        _logger = logger;
    }

    /// <summary>
    /// Post file to mongo
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpGet("Jonins")]
    public IActionResult GetAll ()
    {
        try {
            return Ok();

        } catch (Exception ex) {

            // An error has occured
            // Return 500
            return BadRequest($"Could not upload file: {ex.Message}");
        }        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="birthday"></param>
    /// <returns></returns>
    [HttpPost("InsertJonin")]
    public IActionResult InsertJonin(string firstName, string lastName, string birthday)
    {
        try
        {
            DateTime birthDay = DateTime.Parse(birthday);
            Jonin jonin = new Jonin()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDay

            };

            if (_mockService.InserJonin(jonin))            
                return Ok($"{jonin.FirstName} {jonin.LastName} is added as Jonin");
            
            else
                return BadRequest($"{jonin.FirstName} {jonin.LastName} was not added");
        }
        catch (Exception ex)
        {

            // An error has occured
            // Return 500
            return BadRequest($"Could not insert Jonin: {ex.Message}");
        }
    }

    /// <summary>
    /// Delete Jonin by Id
    /// </summary>
    /// <param name="id" example="6B29FC40-CA47-1067-B31D-00DD010662DA"></param>
    /// <returns></returns>
    [HttpPost("DeleteJoninById")]
    public IActionResult DeleteJoninById(string id)
    {
        try
        {
            Guid guid = Guid.Parse(id);
            if (guid != Guid.Empty)
            {
                if (_mockService.DeleteJoninByName(guid))
                    return Ok($"{guid} is deleted");
                else
                    return BadRequest($"Could not delete Jonin with {id}");
            }
            return BadRequest($"Id is empty");
        }
        catch (Exception ex)
        {
            // An error has occured
            // Return 500
            return BadRequest($"Could not delete jonin: {ex.Message}");
        }

    }

    /// <summary>
    /// Update Jonin by Id
    /// </summary>
    /// <param name="id" example="6B29FC40-CA47-1067-B31D-00DD010662DA"></param>
    /// <returns></returns>
    [HttpPatch("UpdateJoninById")]
    public IActionResult UpdateJoninById(string id, string? firstName, string? lastName, string? dateTime)
    {
        try
        {
            Guid guid = Guid.Parse(id);
            if (guid != Guid.Empty)
            {
                if (dateTime != null)
                {
                    DateTime birthDay = DateTime.Parse(dateTime);
                    _mockService.UpdateJoninByName(guid, firstName, lastName, birthDay);
                    return Ok($"Updated Jonin with Id {id}");
                }
                else
                {
                    _mockService.UpdateJoninByName(guid, firstName, lastName, null);
                    return Ok($"Updated Jonin with Id {id}");
                }
            }

            return BadRequest("Id is empty");
        }
        catch (Exception ex)
        {
            // An error has occured
            // Return 500
            return BadRequest($"Could not update jonin: {ex.Message}");
        }
    }

}