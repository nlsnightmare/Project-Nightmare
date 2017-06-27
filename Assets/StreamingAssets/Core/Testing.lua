function onInteract()
    d = { "Hello, {S_playerName}! It's good to see you!","fast"}
    core.ShowDialogue(d)
end

function onLoad(name)
    core.Print(name)
    if name == "Town1" then
	tankParams = {
	    image = __dir.."/tank.png",
	    pixelsPerUnit = 247,
	    x = 3,
	    y = 3
	}
        tank = core.Create(tankParams)
	t = {}
	t.test = "A"
	t.what = "b:"
	t.f = "A"
	t.three = "A"
        tank.Bind("onInteract",onInteract)
        tank.Interact()
    end
end

