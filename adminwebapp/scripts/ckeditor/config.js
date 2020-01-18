/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_BR;  
	
	config.language = 'ko';
	config.font_names = "굴림/굴림;굴림체/굴림체;" + config.font_names;
	//alert(config.font_names);
	config.toolbar = 'Full';
	
	config.toolbar_Full = [
      ['Source','-','NewPage','Preview','-','Templates'],
      ['Cut','Copy','Paste','PasteText','PasteFromWord','-'],
      ['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],      
       '/',
      ['Bold','Italic','Underline','Strike','-','Subscript','Superscript'],
       ['NumberedList','BulletedList','-','Outdent','Indent','Blockquote'],
       ['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
       ['Link','Unlink','Anchor'],
      ['Image','Flash','Table','HorizontalRule','Smiley','SpecialChar','PageBreak'],
      '/',
        ['Styles','Format','Font','FontSize'],
       ['TextColor','BGColor']
    ];
	
	
};
