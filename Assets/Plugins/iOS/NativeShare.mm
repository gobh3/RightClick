#import <UIKit/UIKit.h>

extern "C" void ShowShareSheet(const char* text, const char* imagePath)
{
    NSString* message = [NSString stringWithUTF8String:text];
    NSString* imagePathStr = [NSString stringWithUTF8String:imagePath];
    NSMutableArray* items = [NSMutableArray arrayWithObject:message];

    if ([[NSFileManager defaultManager] fileExistsAtPath:imagePathStr]) {
        UIImage* image = [UIImage imageWithContentsOfFile:imagePathStr];
        if (image) {
            [items addObject:image];
        }
    }

    dispatch_async(dispatch_get_main_queue(), ^{
        UIViewController* root = UIApplication.sharedApplication.keyWindow.rootViewController;
        UIActivityViewController* activityVC = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:nil];

        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad) {
            activityVC.popoverPresentationController.sourceView = root.view;
            activityVC.popoverPresentationController.sourceRect = CGRectMake(CGRectGetMidX(root.view.bounds), CGRectGetMidY(root.view.bounds), 0, 0);
        }

        [root presentViewController:activityVC animated:YES completion:nil];
    });
}
