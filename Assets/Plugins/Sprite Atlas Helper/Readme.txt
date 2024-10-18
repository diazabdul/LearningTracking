[Generate Sprite Atlas]
-Create AtlasList file via Create->STVR->Sprite Atlas Helper->Create new Sprite Atlas List
-Put newly created atlaslist object into Resources/Atlas/, create necessary folder if not exist.
-Click 'Initialize Sprite Atlas' to get all available sprite atlas in project.

[Usage]
- If used in a canvas image, put Ui Sprite Atlas Instancer in same GameObject with Image component.
- If used in a sprite renderer, put Sprite Atlas Instancer in same gameObject with Sprite Renderer component.
- Click 'Find Sprites' to open search window.

[Limitation]
- This asset has partial support for sliced sprite into multiple parts, therefore as long sprite naming does not change at all, it will support just fine, otherwise you might need to change a little bit code.