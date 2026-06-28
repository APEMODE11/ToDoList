
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using ToDoList.Data;
using ToDoList.Models;

public class ToDoItemController : Controller
{
    private readonly ApplicationDbContext _context;

    public ToDoItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: TODOITEMS
    public async Task<IActionResult> Index()    
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Filtrování poznámek jen pro aktuálního uživatele
        var userNotes = await _context.ToDoItem
                                      .Where(i => i.UserId == userId)
                                      .ToListAsync();

        return View(userNotes);
    }

    // GET: TODOITEMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var todoitem = await _context.ToDoItem.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

        if (todoitem == null)
        {
            return NotFound();
        }

        return View(todoitem);
    }

    // GET: TODOITEMS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TODOITEMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,TimeDue,IsCompleted,Details")] ToDoItem todoitem)
    {

        if (ModelState.IsValid)
        {
            todoitem.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            todoitem.UserId = userId; // Přiřazení ID k poznámce
            _context.Add(todoitem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(todoitem);
    }

    // GET: TODOITEMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoitem = await _context.ToDoItem.FindAsync(id);
        if (todoitem == null)
        {
            return NotFound();
        }
        return View(todoitem);
    }

    // POST: TODOITEMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,TimeDue,IsCompleted,Details")] ToDoItem todoitem)
    {
        if (id != todoitem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // 1. Najdeme existující záznam v databázi
            var existingItem = await _context.ToDoItem
                .FirstOrDefaultAsync(m => m.Id == id);

            // 2. Bezpečnostní kontrola: Musí patřit přihlášenému uživateli
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (existingItem == null || existingItem.UserId != currentUserId)
            {
                return NotFound();
            }

            // 3. Přepíšeme pouze povolená pole (Title, Content atd.)
            existingItem.Title = todoitem.Title;
            existingItem.Details = todoitem.Details;
            existingItem.TimeDue = todoitem.TimeDue;
            existingItem.IsCompleted = todoitem.IsCompleted;

            // 4. UserId se vůbec nedotkneme, zůstane takové, jaké bylo!

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(todoitem);
    }

    // GET: TODOITEMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoitem = await _context.ToDoItem
            .FirstOrDefaultAsync(m => m.Id == id);
        if (todoitem == null)
        {
            return NotFound();
        }

        return View(todoitem);
    }

    // POST: TODOITEMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var todoitem = await _context.ToDoItem.FindAsync(id);
        if (todoitem != null)
        {
            _context.ToDoItem.Remove(todoitem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ToDoItemExists(int? id)
    {
        return _context.ToDoItem.Any(e => e.Id == id);
    }
}
