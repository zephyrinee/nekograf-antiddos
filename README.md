<p align="left">
  <b>⚒️ Code Snippet</b>
</p>

```js
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<ProtectionService>();
}

public void Configure(IApplicationBuilder app)
{
    app.UseMiddleware<DDoSMiddleware>();
}

// FOR WEBSITE API
const rateLimit = new Map();

function checkRateLimit(ip, limit = 100, windowMs = 60000) {
    const now = Date.now();
    const windowStart = now - windowMs;
    
    if (!rateLimit.has(ip)) {
        rateLimit.set(ip, [now]);
        return true;
    }
    
    const timestamps = rateLimit.get(ip);
    const recentCalls = timestamps.filter(t => t > windowStart);
    
    if (recentCalls.length >= limit) {
        return false;
    }
    
    recentCalls.push(now);
    rateLimit.set(ip, recentCalls);
    return true;
}

// FOR EXPRESS.JS
function ddosMiddleware(req, res, next) {
    const clientIP = req.ip || req.connection.remoteAddress;
    
    if (!checkRateLimit(clientIP)) {
        return res.status(429).json({
            error: 'Too many requests',
            message: 'Please try again later'
        });
    }
    
    next();
}

setInterval(() => {
    const oneHourAgo = Date.now() - 3600000;
    for (const [ip, timestamps] of rateLimit.entries()) {
        const recent = timestamps.filter(t => t > oneHourAgo);
        if (recent.length === 0) {
            rateLimit.delete(ip);
        } else {
            rateLimit.set(ip, recent);
        }
    }
}, 3600000);

public class RequestRateLimiter
{
    private ConcurrentDictionary<string, List<DateTime>> requestLog = new();
    
    public bool IsAllowed(string ip, int maxRequests = 100, TimeSpan window = default)
    {
        if (window == default) window = TimeSpan.FromMinutes(1);
        
        var now = DateTime.Now;
        var windowStart = now - window;
        
        var requests = requestLog.GetOrAdd(ip, new List<DateTime>());
        
        // Hapus request lama
        requests.RemoveAll(t => t < windowStart);
        
        if (requests.Count >= maxRequests)
            return false;
            
        requests.Add(now);
        return true;
    }
}

[ApiController]
public class ApiController : ControllerBase
{
    private static RequestRateLimiter limiter = new();
    
    [HttpGet("api/data")]
    public IActionResult GetData()
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        
        if (!limiter.IsAllowed(clientIp))
        {
            return StatusCode(429, new { error = "Rate limit exceeded" });
        }
        
        return Ok(new { data = "protected content" });
    }
}
```

---

<p align="center">
  <img alt="title" src="https://raw.githubusercontent.com/zephyrinee/zephyrinee/refs/heads/main/images.png" />
</p>

<p align="center">
  <b>Ahmad Azizie Adnan</b><br>
  Automation Bot Developer focused on WhatsApp & Telegram
</p>

---

<p align="left">
  <b>🛠️ Skills & Technologies</b>
</p>

<p align="left">
  <img height="36" src="https://skillicons.dev/icons?i=js,ts,c,html,css" />
</p>

---

<p align="left">
  <b>👤 About Me</b>
</p>

<ul>
  <li><b>Automation Bot Developer</b> focused on building and maintaining WhatsApp & Telegram bots</li>
  <li>Experienced in developing <b>automation systems</b> and <b>multi-feature bots</b></li>
  <li>Building <b>custom messaging solutions</b> using <b>JavaScript and Python</b></li>
  <li>Strong focus on <b>backend logic</b>, command-based systems, and API integrations</li>
  <li>Projects mainly target <b>real-world use cases</b> such as moderation, utilities, and auto-response systems</li>
  <li>Emphasis on <b>clean code</b>, <b>stability</b>, and <b>scalable automation</b></li>
</ul>

---

<p align="left">
  <b>🎯 Current Focus</b>
</p>

<ul>
  <li>Developing scalable and reliable WhatsApp & Telegram bot systems</li>
  <li>Improving automation workflows and command-based architectures</li>
  <li>Building custom bot solutions for real-world messaging needs</li>
  <li>Enhancing bot performance, stability, and maintainability</li>
</ul>

---

<p align="left">
  <b>🚀 Project Highlights</b>
</p>

<ul>
  <li><b>Multi-Feature WhatsApp Bot</b> – Command system, auto-reply, moderation tools, and API integration</li>
  <li><b>Telegram Automation Bot</b> – Messaging automation, user interaction handling, and custom utilities</li>
  <li><b>Bot Workflow Engine</b> – Modular backend logic for scalable and maintainable bot development</li>
  <li><b>Custom Messaging Tools</b> – Tailored bot solutions for specific automation requirements</li>
</ul>

<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&height=120&color=0:00c853,100:0a8f3c&section=footer" />
</p>