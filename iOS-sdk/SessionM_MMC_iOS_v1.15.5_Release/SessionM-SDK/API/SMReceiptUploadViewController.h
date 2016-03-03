//
//  SMReceiptUploadViewController.h
//  SessionM
//
//  Copyright (c) 2016 SessionM. All rights reserved.
//

#import <UIKit/UIKit.h>

/*!
 @class SMReceiptUploadViewController
 @abstract View controller for receipt upload activity.
 @discussion An instance of this class is created and presented by the SDK when the user is asked to upload a receipt image for particular ads. The developer can optionally create an instance using the @link initWithAttributes: @/link method to upload receipt images without going through the ad flow.
 */
@interface SMReceiptUploadViewController : UIViewController

/*!
 @abstract Creates and returns a new instance of <code>SMReceiptUploadViewController</code> with the specified attributes.
 @param attributes Metadata the user should enter when uploading a receipt.
 @result An instance of <code>SMReceiptUploadViewController</code> with the specified attributes.
 */
- (instancetype)initWithAttributes:(NSDictionary *)attributes;
/*!
 @abstract Sets the color scheme of the view controller.
 @discussion This method should be called before an instance of <code>SMReceiptUploadViewController</code> is presented. A value of <code>nil</code> can be specified to keep the current color for a parameter.
 @param bgColor Background color (default is white).
 @param titleColor Title text color (default is black).
 @param descriptionColor Non-title text color (default is black).
 @param systemButtonColor System button color (default is system tint color).
 @param actionButtonColor Action button color (default is blue).
 @param actionButtonTitleColor Action button text color (default is white).
 */
+ (void)setBackgroundColor:(UIColor *)bgColor
                titleColor:(UIColor *)titleColor
          descriptionColor:(UIColor *)descriptionColor
         systemButtonColor:(UIColor *)systemButtonColor
         actionButtonColor:(UIColor *)actionButtonColor
    actionButtonTitleColor:(UIColor *)actionButtonTitleColor;

@end
