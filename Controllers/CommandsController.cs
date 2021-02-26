using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    //api/controller
    [Route("api/commands")] //<-- controller level route
    [ApiController] // <--- this is an attribute which gives you out the box behaviors
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;  //this uses the decoupled version of repo that has any implementation
        private readonly IMapper _mapper;  // automapper

        public CommandsController(ICommanderRepo repository, IMapper mapper) // (.... , mapper) --> use automapper within our controller *this is depedency injection again XD
        {
            //Constructor for depedency injection

            //whatever is dependency injected we want to assign it to _repository
            _repository = repository;
            _mapper = mapper;
        }

        //private readonly MockCommanderRepo _repository = new MockCommanderRepo(); //this uses the mock commander repo w/o dependency injection
        
        //GET api/commands
        [HttpGet]
        public ActionResult <IEnumerable<CommandReadDto>> GetAllCommands()  // changed to IEnumerable of CommandReadDtos
        {
            //hold results
            var commandItems = _repository.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems)); // maps commandItems from our repository to an CommandreadDto
        }


        //GET api/commands/{id}
        [HttpGet("{id}", Name="GetCommandById")] //binding source   // Name="GetCommandById  == makes use of this method in another method??  has to be same name as method name
        public ActionResult <CommandReadDto> GetCommandById(int id) //change <Command> to CommandReadDto
        {
            var commandItem = _repository.GetCommandById(id);
            if(commandItem != null)
            {
                // return commandreaddto thats been mapped
                // mapping data from commandItem to a CommandReadDto
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }
            return NotFound();
            
        }

        //==================================
        //POST api/commands
        [HttpPost]  //--> Makes it unique cuz the verb tells the controller that its a post
        public ActionResult <CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            // put whatever we get into our api request body and convert it into a model to then be put into the repository

            var commandModel = _mapper.Map<Command>(commandCreateDto);

            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();


            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            //  // returns commandModel object...we wanna return a CommandReadDto
            //return Ok(commandReadDto);


            // CreatedAtRoute( routeName, routeValues, content)
            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto);


        }

        //=====================================
        // PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)  // we are returning a NoContent so we do not need a return type
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, commandModelFromRepo); //basically updated the model and tracked from dbcontext so didnt need an update implementation


            //call update method on respository supply our commandModelFromRepo

            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent(); //return a 204


        }


        //===============================
        // PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            // check to see if the source exists
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            // we are getting a commandUpdateDto patchDoc and want to apply it directly to our commandmodel.
            // we cant apply the patch directly to the model
            // need to make a commandUpdateDto so we can apply the commandModel to the CommandUpdateDto

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);// creating a new CommandUpdateDto with the contents from the Repo makes use of CreateMap<Command,CommandUpdateDto>() in CommandsProfile.cs

            //make use of patchDoc
            patchDoc.ApplyTo(commandToPatch, ModelState);  //ModelState makes sure the validations are valid

            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // so after validations check out we now want to update
            _mapper.Map(commandToPatch, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        //=============================
        // DELETE api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}