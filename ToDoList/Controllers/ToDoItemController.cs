
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.Data;

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
        return View(await _context.ToDoItem.ToListAsync());
    }

    // GET: TODOITEMS/Details/5
    public async Task<IActionResult> Details(int? id)
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
            try
            {
                _context.Update(todoitem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(todoitem.Id))
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
