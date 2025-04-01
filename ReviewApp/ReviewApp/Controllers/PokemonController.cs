using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Repository;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository pokemonRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IOwnerRepository ownerRepository;
        private readonly IMapper mapper;
        public PokemonController(IPokemonRepository pokemonRepository, ICategoryRepository categoryRepository, IOwnerRepository ownerRepository, IMapper mapper) 
        {
            this.pokemonRepository = pokemonRepository;
            this.categoryRepository = categoryRepository;
            this.ownerRepository = ownerRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = mapper.Map<List<PokemonDto>>(pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId) 
        { 
            if (!pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = mapper.Map<PokemonDto>(pokemonRepository.GetPokemon(pokeId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var owner = ownerRepository.GetOwner(ownerId);

            if (owner == null)
            {
                ModelState.AddModelError("", "Owner not found");
                return StatusCode(422, ModelState);
            }

            var category = categoryRepository.GetCategory(categoryId);

            if (category == null)
            {
                ModelState.AddModelError("", "category not found");
                return StatusCode(422, ModelState);
            }

            var pokemonMap = mapper.Map<Pokemon>(pokemonCreate);

            if (!pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully create");
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokemonId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokemonId != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!categoryRepository.CategoryExists(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = mapper.Map<Pokemon>(updatedPokemon);

            if (!pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
