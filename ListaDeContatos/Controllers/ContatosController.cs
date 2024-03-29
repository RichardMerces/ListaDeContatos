﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ListaDeContatos.Data;
using ListaDeContatos.Models;

namespace ListaDeContatos.Controllers
{
    public class ContatosController : Controller
    {
        private readonly ContatoContext _context;

		public ContatosController(ContatoContext context)
        {
            _context = context;
        }

        // GET: Contatos
        public async Task<IActionResult> Index()
        {
              return _context.Contatos != null ? 
                          View(await _context.Contatos.ToListAsync()) :
                          Problem("Entity set 'ContatoContext.Contatos'  is null.");
        }

        // GET: Contatos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contatos == null)
            {
                return NotFound();
            }

            var contatos = await _context.Contatos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contatos == null)
            {
                return NotFound();
            }

            return View(contatos);
        }

        // GET: Contatos/Filter
        public async Task<IActionResult> Filter(string name, string email, string phone)
        {
            IQueryable<Contatos> contatosQuery = _context.Contatos;

            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                contatosQuery = contatosQuery.Where(c => c.Name.ToLower().Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                email = email.ToLower();
                contatosQuery = contatosQuery.Where(c => c.Email.ToLower().Contains(email));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                phone = phone.ToLower();
                contatosQuery = contatosQuery.Where(c => c.Phone.ToLower().Contains(phone));
            }

            List<Contatos> contatos = await contatosQuery.ToListAsync();
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(phone))
            {
                return View();
            }

            return View("Index", contatos);
        }

        // GET: Contatos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contatos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone")] Contatos contatos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contatos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contatos);
        }

        // GET: Contatos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contatos == null)
            {
                return NotFound();
            }

            var contatos = await _context.Contatos.FindAsync(id);
            if (contatos == null)
            {
                return NotFound();
            }
            return View(contatos);
        }

        // POST: Contatos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone")] Contatos contatos)
        {
            if (id != contatos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contatos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContatosExists(contatos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contatos);
        }

        // GET: Contatos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contatos == null)
            {
                return NotFound();
            }

            var contatos = await _context.Contatos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contatos == null)
            {
                return NotFound();
            }

            return View(contatos);
        }

        // POST: Contatos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contatos == null)
            {
                return Problem("Entity set 'ContatoContext.Contatos'  is null.");
            }
            var contatos = await _context.Contatos.FindAsync(id);
            if (contatos != null)
            {
                _context.Contatos.Remove(contatos);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContatosExists(int id)
        {
          return (_context.Contatos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
