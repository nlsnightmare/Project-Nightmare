function onLoad()
    core.Print("onLoad is running...")
    core.LoadSprite(__dir)
    core.ShowDialogue({ "Hello world!" , "this is lua motherfucker" }, "normal")
end
