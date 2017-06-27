function onInteract()
    d = { "Hello, {S_playerName}! It's good to see you!","fast"}
    core.ShowDialogue(d)
end

function onLoad(name)
    core.Print(name)
    if name == "Town1" then
        tank = core.Create(__dir.."/tank.png",247,3,3)
	t = {}
	t.test = "A"
	t.what = "b:"
	t.f = "A"
	t.three = "A"
        tank.Bind("onInteract",onInteract)
        tank.Interact()
    end
end

