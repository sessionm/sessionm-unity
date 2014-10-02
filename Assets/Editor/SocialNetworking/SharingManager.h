//
//  SharingManager.h
//  Unity-iPhone
//
//  Created by Mike Desaro on 1/28/13.
//
//

#import <Foundation/Foundation.h>


@interface SharingManager : NSObject


+ (SharingManager*)sharedManager;

+ (void)shareItems:(NSArray*)items excludedActivityTypes:(NSArray*)excludedActivityTypes;



@end
